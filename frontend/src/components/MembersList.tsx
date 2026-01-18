import { useState, useEffect } from 'react';
import { membersApi, Member } from '../api';

interface MembersListProps {
  groupId: string;
  onUpdate?: () => void;
}

export default function MembersList({ groupId, onUpdate }: MembersListProps) {
  const [members, setMembers] = useState<Member[]>([]);
  const [loading, setLoading] = useState(true);
  const [showAddForm, setShowAddForm] = useState(false);
  const [newMember, setNewMember] = useState({ name: '', email: '' });

  useEffect(() => {
    loadMembers();
  }, [groupId]);

  const loadMembers = async () => {
    try {
      setLoading(true);
      const data = await membersApi.getByGroup(groupId);
      setMembers(data);
    } catch (err) {
      console.error('Failed to load members:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddMember = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newMember.name.trim() || !newMember.email.trim()) return;

    try {
      const member = await membersApi.create(groupId, newMember.name, newMember.email);
      setMembers([...members, member]);
      setNewMember({ name: '', email: '' });
      setShowAddForm(false);
      onUpdate?.();
    } catch (err) {
      alert('Failed to add member');
      console.error(err);
    }
  };

  const handleDeleteMember = async (id: string) => {
    if (!confirm('Are you sure you want to remove this member?')) return;

    try {
      await membersApi.delete(id);
      setMembers(members.filter(m => m.id !== id));
      onUpdate?.();
    } catch (err) {
      alert('Failed to delete member');
      console.error(err);
    }
  };

  if (loading) {
    return <div className="text-center py-4">Loading members...</div>;
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h3 className="text-lg font-semibold text-gray-800">Group Members</h3>
        <button
          onClick={() => setShowAddForm(!showAddForm)}
          className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md text-sm font-medium"
        >
          + Add Member
        </button>
      </div>

      {showAddForm && (
        <form onSubmit={handleAddMember} className="bg-gray-50 p-4 rounded-lg mb-4">
          <div className="grid grid-cols-2 gap-3 mb-3">
            <input
              type="text"
              value={newMember.name}
              onChange={(e) => setNewMember({ ...newMember, name: e.target.value })}
              placeholder="Name"
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
            <input
              type="email"
              value={newMember.email}
              onChange={(e) => setNewMember({ ...newMember, email: e.target.value })}
              placeholder="Email"
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
          </div>
          <div className="flex gap-2">
            <button
              type="submit"
              className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-md font-medium"
            >
              Add
            </button>
            <button
              type="button"
              onClick={() => {
                setShowAddForm(false);
                setNewMember({ name: '', email: '' });
              }}
              className="bg-gray-300 hover:bg-gray-400 text-gray-700 px-4 py-2 rounded-md font-medium"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      {members.length === 0 ? (
        <div className="text-center py-12 text-gray-500">
          <div className="text-5xl mb-3">👤</div>
          <p className="text-lg">No members yet</p>
          <p className="text-sm">Add members to start splitting expenses</p>
        </div>
      ) : (
        <div className="space-y-3">
          {members.map((member) => (
            <div
              key={member.id}
              className="bg-gray-50 p-4 rounded-lg flex justify-between items-center hover:bg-gray-100 transition-colors"
            >
              <div>
                <h4 className="font-semibold text-gray-900">{member.name}</h4>
                <p className="text-sm text-gray-600">{member.email}</p>
              </div>
              <button
                onClick={() => handleDeleteMember(member.id)}
                className="text-red-500 hover:text-red-700 px-3 py-1 rounded"
                title="Remove member"
              >
                🗑️
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
