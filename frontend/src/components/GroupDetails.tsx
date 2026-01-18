import { useState, useEffect } from 'react';
import { Group, groupsApi } from '../api';
import MembersList from './MembersList';
import ExpensesList from './ExpensesList';
import BalancesView from './BalancesView';

interface GroupDetailsProps {
  groupId: string;
}

export default function GroupDetails({ groupId }: GroupDetailsProps) {
  const [group, setGroup] = useState<Group | null>(null);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'members' | 'expenses' | 'balances'>('members');
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    loadGroup();
  }, [groupId, refreshKey]);

  const loadGroup = async () => {
    try {
      setLoading(true);
      const data = await groupsApi.getById(groupId);
      setGroup(data);
    } catch (err) {
      console.error('Failed to load group:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleRefresh = () => {
    setRefreshKey(prev => prev + 1);
  };

  if (loading) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6">
        <div className="animate-pulse">
          <div className="h-8 bg-gray-200 rounded w-1/2 mb-6"></div>
          <div className="h-64 bg-gray-200 rounded"></div>
        </div>
      </div>
    );
  }

  if (!group) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6">
        <p className="text-red-500">Group not found</p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md">
      {/* Header */}
      <div className="p-6 border-b border-gray-200">
        <h2 className="text-2xl font-bold text-gray-900">{group.name}</h2>
        <p className="text-sm text-gray-500 mt-1">
          {group.currency} • {group.members.length} members • {group.expenseCount} expenses
        </p>
      </div>

      {/* Tabs */}
      <div className="border-b border-gray-200">
        <nav className="flex -mb-px">
          <button
            onClick={() => setActiveTab('members')}
            className={`px-6 py-3 text-sm font-medium border-b-2 transition-colors ${
              activeTab === 'members'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            👥 Members
          </button>
          <button
            onClick={() => setActiveTab('expenses')}
            className={`px-6 py-3 text-sm font-medium border-b-2 transition-colors ${
              activeTab === 'expenses'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            💳 Expenses
          </button>
          <button
            onClick={() => setActiveTab('balances')}
            className={`px-6 py-3 text-sm font-medium border-b-2 transition-colors ${
              activeTab === 'balances'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            ⚖️ Balances
          </button>
        </nav>
      </div>

      {/* Content */}
      <div className="p-6">
        {activeTab === 'members' && <MembersList groupId={groupId} onUpdate={handleRefresh} />}
        {activeTab === 'expenses' && <ExpensesList groupId={groupId} onUpdate={handleRefresh} />}
        {activeTab === 'balances' && <BalancesView groupId={groupId} />}
      </div>
    </div>
  );
}
