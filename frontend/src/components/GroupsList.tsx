import { useState, useEffect } from 'react';
import { groupsApi, Group } from '../api';

interface GroupsListProps {
  selectedGroupId: string | null;
  onSelectGroup: (id: string) => void;
}

export default function GroupsList({ selectedGroupId, onSelectGroup }: GroupsListProps) {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [newGroupName, setNewGroupName] = useState('');
  const [newGroupCurrency, setNewGroupCurrency] = useState('USD');

  useEffect(() => {
    loadGroups();
  }, []);

  const loadGroups = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await groupsApi.getAll();
      setGroups(data);
    } catch (err) {
      setError('Failed to load groups');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateGroup = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newGroupName.trim()) return;

    try {
      const newGroup = await groupsApi.create(newGroupName, newGroupCurrency);
      setGroups([...groups, newGroup]);
      setNewGroupName('');
      setNewGroupCurrency('USD');
      setShowCreateForm(false);
      onSelectGroup(newGroup.id);
    } catch (err) {
      alert('Failed to create group');
      console.error(err);
    }
  };

  const handleDeleteGroup = async (id: string, e: React.MouseEvent) => {
    e.stopPropagation();
    if (!confirm('Are you sure you want to delete this group?')) return;

    try {
      await groupsApi.delete(id);
      setGroups(groups.filter(g => g.id !== id));
      if (selectedGroupId === id) {
        onSelectGroup(groups.find(g => g.id !== id)?.id || '');
      }
    } catch (err) {
      alert('Failed to delete group');
      console.error(err);
    }
  };

  if (loading) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6">
        <div className="animate-pulse">
          <div className="h-8 bg-gray-200 rounded w-3/4 mb-4"></div>
          <div className="space-y-3">
            <div className="h-12 bg-gray-200 rounded"></div>
            <div className="h-12 bg-gray-200 rounded"></div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-xl font-bold text-gray-800">Groups</h2>
        <button
          onClick={() => setShowCreateForm(!showCreateForm)}
          className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md text-sm font-medium transition-colors"
        >
          + New
        </button>
      </div>

      {showCreateForm && (
        <form onSubmit={handleCreateGroup} className="mb-4">
          <div className="space-y-2 mb-2">
            <input
              type="text"
              value={newGroupName}
              onChange={(e) => setNewGroupName(e.target.value)}
              placeholder="Group name..."
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              autoFocus
            />
            <select
              value={newGroupCurrency}
              onChange={(e) => setNewGroupCurrency(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="USD">USD</option>
              <option value="EUR">EUR</option>
              <option value="GBP">GBP</option>
              <option value="TRY">TRY</option>
            </select>
          </div>
          <div className="flex gap-2">
            <button
              type="submit"
              className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-md font-medium"
            >
              Create
            </button>
            <button
              type="button"
              onClick={() => {
                setShowCreateForm(false);
                setNewGroupName('');
                setNewGroupCurrency('USD');
              }}
              className="bg-gray-300 hover:bg-gray-400 text-gray-700 px-4 py-2 rounded-md font-medium"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      {error && (
        <div className="bg-red-50 text-red-600 p-3 rounded-md mb-4">
          {error}
        </div>
      )}

      <div className="space-y-2">
        {groups.length === 0 ? (
          <div className="text-center py-8 text-gray-500">
            <div className="text-4xl mb-2">📁</div>
            <p>No groups yet</p>
            <p className="text-sm">Create your first group to get started</p>
          </div>
        ) : (
          groups.map((group) => (
            <div
              key={group.id}
              onClick={() => onSelectGroup(group.id)}
              className={`p-4 rounded-lg cursor-pointer transition-all ${
                selectedGroupId === group.id
                  ? 'bg-blue-100 border-2 border-blue-500'
                  : 'bg-gray-50 hover:bg-gray-100 border-2 border-transparent'
              }`}
            >
              <div className="flex justify-between items-center">
                <div>
                  <h3 className="font-semibold text-gray-900">{group.name}</h3>
                  <p className="text-xs text-gray-500">{group.currency} • {group.expenseCount} expenses</p>
                </div>
                <button
                  onClick={(e) => handleDeleteGroup(group.id, e)}
                  className="text-red-500 hover:text-red-700 px-2 py-1 rounded"
                  title="Delete group"
                >
                  🗑️
                </button>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}
