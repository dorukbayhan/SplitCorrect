import { useState } from 'react';
import GroupsList from './components/GroupsList';
import GroupDetails from './components/GroupDetails';

function App() {
  const [selectedGroupId, setSelectedGroupId] = useState<string | null>(null);

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <header className="bg-white shadow-md">
        <div className="max-w-7xl mx-auto px-4 py-6 sm:px-6 lg:px-8">
          <h1 className="text-3xl font-bold text-gray-900">
            💰 SplitCorrect
          </h1>
          <p className="text-gray-600 mt-1">Split expenses fairly with friends</p>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 py-8 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Groups List */}
          <div className="lg:col-span-1">
            <GroupsList 
              selectedGroupId={selectedGroupId}
              onSelectGroup={setSelectedGroupId}
            />
          </div>

          {/* Group Details */}
          <div className="lg:col-span-2">
            {selectedGroupId ? (
              <GroupDetails groupId={selectedGroupId} />
            ) : (
              <div className="bg-white rounded-lg shadow-md p-12 text-center">
                <div className="text-6xl mb-4">👈</div>
                <h2 className="text-xl font-semibold text-gray-700 mb-2">
                  Select a Group
                </h2>
                <p className="text-gray-500">
                  Choose a group from the list to view details, members, and expenses
                </p>
              </div>
            )}
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
