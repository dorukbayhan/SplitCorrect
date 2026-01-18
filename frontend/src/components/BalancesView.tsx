import { useState, useEffect } from 'react';
import { expensesApi, GroupDetail } from '../api';

interface BalancesViewProps {
  groupId: string;
}

export default function BalancesView({ groupId }: BalancesViewProps) {
  const [groupDetails, setGroupDetails] = useState<GroupDetail | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, [groupId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const data = await expensesApi.getGroupDetails(groupId);
      setGroupDetails(data);
    } catch (err) {
      console.error('Failed to load balances:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="text-center py-4">Loading balances...</div>;
  }

  if (!groupDetails || groupDetails.balances.length === 0) {
    return (
      <div className="text-center py-12 text-gray-500">
        <div className="text-5xl mb-3">💰</div>
        <p className="text-lg">No balances to show</p>
        <p className="text-sm">Add expenses to see balances</p>
      </div>
    );
  }

  const { balances, settlements } = groupDetails;

  return (
    <div className="space-y-6">
      {/* Balances */}
      <div>
        <h3 className="text-lg font-semibold text-gray-800 mb-4">Member Balances</h3>
        <div className="space-y-3">
          {balances.map((balance) => {
            const isPositive = balance.amount > 0;
            const isZero = balance.amount === 0;
            
            return (
              <div
                key={balance.memberId}
                className="bg-gray-50 p-4 rounded-lg flex justify-between items-center"
              >
                <div>
                  <h4 className="font-semibold text-gray-900">{balance.memberName}</h4>
                </div>
                <div className="text-right">
                  <div
                    className={`text-xl font-bold ${
                      isZero ? 'text-gray-500' : isPositive ? 'text-green-600' : 'text-red-600'
                    }`}
                  >
                    {isPositive ? '+' : ''}{balance.amount.toFixed(2)} {balance.currency}
                  </div>
                  <p className="text-xs text-gray-500">
                    {isZero ? 'Settled up' : isPositive ? 'Gets back' : 'Owes'}
                  </p>
                </div>
              </div>
            );
          })}
        </div>
      </div>

      {/* Settlement Suggestions */}
      {settlements.length > 0 && (
        <div>
          <h3 className="text-lg font-semibold text-gray-800 mb-4">💡 Settlement Suggestions</h3>
          <p className="text-sm text-gray-600 mb-4">
            These are the minimum transactions needed to settle all balances:
          </p>
          <div className="space-y-3">
            {settlements.map((settlement, index) => (
              <div
                key={index}
                className="bg-gradient-to-r from-blue-50 to-indigo-50 p-4 rounded-lg border-l-4 border-blue-500"
              >
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-3">
                    <div className="bg-white rounded-full p-2">
                      <span className="text-2xl">💸</span>
                    </div>
                    <div>
                      <p className="font-semibold text-gray-900">
                        {settlement.fromMemberName} <span className="text-blue-600">→</span> {settlement.toMemberName}
                      </p>
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="text-xl font-bold text-blue-600">
                      {settlement.amount.toFixed(2)} {settlement.currency}
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
          <div className="mt-4 p-3 bg-green-50 rounded-lg border border-green-200">
            <p className="text-sm text-green-800">
              ✅ Complete these {settlements.length} transaction{settlements.length > 1 ? 's' : ''} to settle all balances
            </p>
          </div>
        </div>
      )}

      {settlements.length === 0 && balances.some(b => b.amount !== 0) && (
        <div className="p-4 bg-yellow-50 rounded-lg border border-yellow-200">
          <p className="text-sm text-yellow-800">
            ⚠️ No settlement suggestions available. This might happen if all members have zero balance.
          </p>
        </div>
      )}

      {balances.every(b => b.amount === 0) && (
        <div className="p-4 bg-green-50 rounded-lg border border-green-200">
          <p className="text-sm text-green-800 text-center">
            🎉 All members are settled up! No outstanding balances.
          </p>
        </div>
      )}
    </div>
  );
}
