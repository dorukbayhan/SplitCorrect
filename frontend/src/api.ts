const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5016/api';

export interface Group {
  id: string;
  name: string;
  currency: string;
  description?: string;
  createdAt: string;
  members: Member[];
  expenseCount: number;
}

export interface Member {
  id: string;
  name: string;
  email: string;
  createdAt: string;
}

export interface Expense {
  id: string;
  description: string;
  amount: number;
  currency: string;
  paidBy: Member;
  expenseDate: string;
  splits: ExpenseSplit[];
}

export interface ExpenseSplit {
  memberId: string;
  memberName: string;
  amount: number;
  currency: string;
}

export interface Balance {
  memberId: string;
  memberName: string;
  amount: number;
  currency: string;
}

export interface Settlement {
  fromMemberId: string;
  fromMemberName: string;
  toMemberId: string;
  toMemberName: string;
  amount: number;
  currency: string;
}

export interface GroupDetail {
  id: string;
  name: string;
  currency: string;
  description?: string;
  createdAt: string;
  members: Member[];
  expenses: Expense[];
  balances: Balance[];
  settlements: Settlement[];
}

// Groups API
export const groupsApi = {
  getAll: async (): Promise<Group[]> => {
    const response = await fetch(`${API_BASE_URL}/groups`);
    if (!response.ok) throw new Error('Failed to fetch groups');
    return response.json();
  },

  getById: async (id: string): Promise<Group> => {
    const response = await fetch(`${API_BASE_URL}/groups/${id}`);
    if (!response.ok) throw new Error('Failed to fetch group');
    return response.json();
  },

  create: async (name: string, currency: string = 'USD'): Promise<Group> => {
    const response = await fetch(`${API_BASE_URL}/groups`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, currency }),
    });
    if (!response.ok) throw new Error('Failed to create group');
    return response.json();
  },

  update: async (id: string, name: string): Promise<Group> => {
    const response = await fetch(`${API_BASE_URL}/groups/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name }),
    });
    if (!response.ok) throw new Error('Failed to update group');
    return response.json();
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/groups/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete group');
  },
};

// Members API
export const membersApi = {
  getByGroup: async (groupId: string): Promise<Member[]> => {
    const response = await fetch(`${API_BASE_URL}/members/group/${groupId}`);
    if (!response.ok) throw new Error('Failed to fetch members');
    return response.json();
  },

  create: async (groupId: string, name: string, email: string): Promise<Member> => {
    const response = await fetch(`${API_BASE_URL}/members`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ groupId, name, email }),
    });
    if (!response.ok) throw new Error('Failed to create member');
    return response.json();
  },

  update: async (id: string, name: string, email: string): Promise<Member> => {
    const response = await fetch(`${API_BASE_URL}/members/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, email }),
    });
    if (!response.ok) throw new Error('Failed to update member');
    return response.json();
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/members/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete member');
  },
};

// Expenses API
export const expensesApi = {
  getByGroup: async (groupId: string): Promise<Expense[]> => {
    const response = await fetch(`${API_BASE_URL}/expenses/group/${groupId}`);
    if (!response.ok) throw new Error('Failed to fetch expenses');
    return response.json();
  },

  create: async (
    groupId: string,
    description: string,
    totalAmount: number,
    currency: string,
    paidByMemberId: string,
    splitBetweenMemberIds?: string[]
  ): Promise<Expense> => {
    const response = await fetch(`${API_BASE_URL}/expenses`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        groupId,
        description,
        totalAmount,
        currency,
        paidByMemberId,
        splitBetweenMemberIds,
      }),
    });
    if (!response.ok) throw new Error('Failed to create expense');
    return response.json();
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/expenses/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete expense');
  },

  getGroupDetails: async (groupId: string): Promise<GroupDetail> => {
    const response = await fetch(`${API_BASE_URL}/expenses/group/${groupId}/details`);
    if (!response.ok) throw new Error('Failed to fetch group details');
    return response.json();
  },
};
