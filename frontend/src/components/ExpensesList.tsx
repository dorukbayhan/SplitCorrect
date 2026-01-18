import { useState, useEffect } from 'react';
import { expensesApi, membersApi, Expense, Member } from '../api';

interface ExpensesListProps {
  groupId: string;
  onUpdate?: () => void;
}

export default function ExpensesList({ groupId, onUpdate }: ExpensesListProps) {
  const [expenses, setExpenses] = useState<Expense[]>([]);
  const [members, setMembers] = useState<Member[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddForm, setShowAddForm] = useState(false);
  const [newExpense, setNewExpense] = useState({
    description: '',
    totalAmount: '',
    currency: 'USD',
    paidByMemberId: '',
    splitBetweenMemberIds: [] as string[],
  });

  useEffect(() => {
    loadData();
  }, [groupId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [expensesData, membersData] = await Promise.all([
        expensesApi.getByGroup(groupId),
        membersApi.getByGroup(groupId),
      ]);
      setExpenses(expensesData);
      setMembers(membersData);
    } catch (err) {
      console.error('Failed to load data:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddExpense = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newExpense.description.trim() || !newExpense.totalAmount || !newExpense.paidByMemberId) {
      alert('Please fill all required fields');
      return;
    }

    try {
      const expense = await expensesApi.create(
        groupId,
        newExpense.description,
        parseFloat(newExpense.totalAmount),
        newExpense.currency,
        newExpense.paidByMemberId,
        newExpense.splitBetweenMemberIds.length > 0 ? newExpense.splitBetweenMemberIds : undefined
      );
      setExpenses([...expenses, expense]);
      setNewExpense({ 
        description: '', 
        totalAmount: '', 
        currency: 'USD', 
        paidByMemberId: '',
        splitBetweenMemberIds: []
      });
      setShowAddForm(false);
      onUpdate?.();
    } catch (err) {
      alert('Failed to add expense');
      console.error(err);
    }
  };

  const toggleSplitMember = (memberId: string) => {
    setNewExpense(prev => ({
      ...prev,
      splitBetweenMemberIds: prev.splitBetweenMemberIds.includes(memberId)
        ? prev.splitBetweenMemberIds.filter(id => id !== memberId)
        : [...prev.splitBetweenMemberIds, memberId]
    }));
  };

  const handleDeleteExpense = async (id: string) => {
    if (!confirm('Are you sure you want to delete this expense?')) return;

    try {
      await expensesApi.delete(id);
      setExpenses(expenses.filter(e => e.id !== id));
      onUpdate?.();
    } catch (err) {
      alert('Failed to delete expense');
      console.error(err);
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString();
  };

  if (loading) {
    return <div className="text-center py-4">Loading expenses...</div>;
  }

  if (members.length === 0) {
    return (
      <div className="text-center py-12 text-gray-500">
        <div className="text-5xl mb-3">👥</div>
        <p className="text-lg">No members in this group</p>
        <p className="text-sm">Add members first before creating expenses</p>
      </div>
    );
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h3 className="text-lg font-semibold text-gray-800">Expenses</h3>
        <button
          onClick={() => setShowAddForm(!showAddForm)}
          className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md text-sm font-medium"
        >
          + Add Expense
        </button>
      </div>

      {showAddForm && (
        <form onSubmit={handleAddExpense} className="bg-gray-50 p-4 rounded-lg mb-4 space-y-4">
          <div className="grid grid-cols-2 gap-3">
            <input
              type="text"
              value={newExpense.description}
              onChange={(e) => setNewExpense({ ...newExpense, description: e.target.value })}
              placeholder="Description (e.g., Dinner)"
              className="col-span-2 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
            <input
              type="number"
              step="0.01"
              value={newExpense.totalAmount}
              onChange={(e) => setNewExpense({ ...newExpense, totalAmount: e.target.value })}
              placeholder="Amount"
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
            <select
              value={newExpense.currency}
              onChange={(e) => setNewExpense({ ...newExpense, currency: e.target.value })}
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="USD">USD</option>
              <option value="EUR">EUR</option>
              <option value="GBP">GBP</option>
              <option value="TRY">TRY</option>
            </select>
            <select
              value={newExpense.paidByMemberId}
              onChange={(e) => setNewExpense({ ...newExpense, paidByMemberId: e.target.value })}
              className="col-span-2 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            >
              <option value="">Select who paid...</option>
              {members.map((member) => (
                <option key={member.id} value={member.id}>
                  {member.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Split between (leave empty to split among all members):
            </label>
            <div className="grid grid-cols-2 gap-2">
              {members.map((member) => (
                <label
                  key={member.id}
                  className="flex items-center space-x-2 p-2 border border-gray-300 rounded-md hover:bg-gray-50 cursor-pointer"
                >
                  <input
                    type="checkbox"
                    checked={newExpense.splitBetweenMemberIds.includes(member.id)}
                    onChange={() => toggleSplitMember(member.id)}
                    className="rounded text-blue-500 focus:ring-2 focus:ring-blue-500"
                  />
                  <span className="text-sm">{member.name}</span>
                </label>
              ))}
            </div>
          </div>

          <div className="flex gap-2">
            <button
              type="submit"
              className="flex-1 bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-md font-medium"
            >
              Add
            </button>
            <button
              type="button"
              onClick={() => {
                setShowAddForm(false);
                setNewExpense({ 
                  description: '', 
                  totalAmount: '', 
                  currency: 'USD', 
                  paidByMemberId: '',
                  splitBetweenMemberIds: []
                });
              }}
              className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-700 px-4 py-2 rounded-md font-medium"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      {expenses.length === 0 ? (
        <div className="text-center py-12 text-gray-500">
          <div className="text-5xl mb-3">💸</div>
          <p className="text-lg">No expenses yet</p>
          <p className="text-sm">Add an expense to start tracking</p>
        </div>
      ) : (
        <div className="space-y-3">
          {expenses.map((expense) => (
            <div
              key={expense.id}
              className="bg-gray-50 p-4 rounded-lg flex justify-between items-center hover:bg-gray-100 transition-colors"
            >
              <div className="flex-1">
                <div className="flex items-center gap-3">
                  <h4 className="font-semibold text-gray-900">{expense.description}</h4>
                  <span className="text-lg font-bold text-green-600">
                    {expense.amount.toFixed(2)} {expense.currency}
                  </span>
                </div>
                <p className="text-sm text-gray-600 mt-1">
                  Paid by <span className="font-medium">{expense.paidBy.name}</span>
                </p>
                <p className="text-xs text-gray-400">
                  {formatDate(expense.expenseDate)} • Split between {expense.splits.length} members
                </p>
              </div>
              <button
                onClick={() => handleDeleteExpense(expense.id)}
                className="text-gray-400 hover:text-gray-600 px-3 py-1 rounded ml-4 transition-colors"
                title="Delete expense"
              >
                🗑️
              </button>
            </div>
          ))}
        </div>
      )}

      {expenses.length > 0 && (
        <div className="mt-6 p-4 bg-blue-50 rounded-lg">
          <div className="flex justify-between items-center">
            <span className="font-semibold text-gray-700">Total Expenses:</span>
            <span className="text-xl font-bold text-blue-600">
              {expenses.reduce((sum, e) => sum + e.amount, 0).toFixed(2)} {expenses[0]?.currency || 'USD'}
            </span>
          </div>
        </div>
      )}
    </div>
  );
}
