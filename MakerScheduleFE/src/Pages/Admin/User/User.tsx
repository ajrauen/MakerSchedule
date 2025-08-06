import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { useAdminUsersData } from "@ms/hooks/useAdminUsersData";
import { useQueryClient } from "@tanstack/react-query";

import { useEffect, useState, type TransitionEvent } from "react";
import type { DomainUser } from "@ms/types/domain-user.types";
import { UserHeader } from "@ms/Pages/Admin/User/Header/UserHeader";
import { AdminUsersTable } from "@ms/Pages/Admin/User/Table/Table";
import { UserDetails } from "@ms/Pages/Admin/User/Details/UserDetails";

const AdminUsers = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [selectedUser, setSelectedUser] = useState<DomainUser | undefined>(
    undefined
  );
  const [userToDelete, setUserToDelete] = useState<DomainUser | undefined>();
  const [filteredUsers, setFilteredUsers] = useState<DomainUser[]>([]);
  const [searchString, setSearchString] = useState("");
  const [filterValue, setFilterValue] = useState("");

  const { users, userMetaData } = useAdminUsersData();
  const queryClient = useQueryClient();

  useEffect(() => {
    if (!users) return;
    setFilteredUsers(users);
  }, [users]);

  const handleUserEdit = (user: DomainUser) => {
    setSelectedUser(user);
    handleDrawerOpen();
  };

  const handleUserCreate = () => {
    const newUser: DomainUser = {
      firstName: "",
      lastName: "",
      id: "",
      roles: [],
    };
    setSelectedUser(newUser);
    handleDrawerOpen();
  };

  const handleDrawerOpen = () => {
    setRefreshData(false);
    setIsDrawerOpen(true);
  };

  const handlePanelTransitionEnd = (event: TransitionEvent<HTMLDivElement>) => {
    if (
      event.propertyName === "transform" &&
      event.target === event.currentTarget &&
      refreshData
    ) {
      queryClient.invalidateQueries({
        queryKey: ["events"],
      });
    }
  };

  const handleDeleteClick = (user: DomainUser) => {
    setUserToDelete(user);
  };

  const handleCancelDeleteUser = () => setUserToDelete(undefined);
  const handleConfirmDeleteUser = () => {
    if (!userToDelete?.id) return;

    // deleteEventMutation(eventToDelete.id);
  };

  const handleSearch = (value: string | undefined) => {
    setSearchString(value || "");
  };

  const handleFilterChange = (value: string) => {
    setFilterValue(value);
  };

  useEffect(() => {
    let filtered =
      users?.filter((user) => {
        return (
          user.firstName?.toLowerCase().includes(searchString.toLowerCase()) ||
          user.lastName?.toLowerCase().includes(searchString.toLowerCase()) ||
          user?.email?.toLowerCase().includes(searchString.toLowerCase())
        );
      }) || [];

    if (filterValue) {
      filtered = filtered.filter((user) => user.roles?.includes(filterValue));
    }

    setFilteredUsers(filtered);
  }, [searchString, filterValue, users]);

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${isDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: isDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <UserHeader
          onCreateEvent={handleUserCreate}
          onSearch={handleSearch}
          onFilterChange={handleFilterChange}
          roles={userMetaData.roles}
        />
        <AdminUsersTable
          users={filteredUsers}
          selectedUser={selectedUser}
          onEdit={handleUserEdit}
          onEventDelete={handleDeleteClick}
        />
      </div>
      <div
        className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
        style={{
          willChange: "transform",
          transform: isDrawerOpen ? "translateX(0)" : "translateX(100%)",
        }}
        onTransitionEnd={handlePanelTransitionEnd}
      >
        <div className="p-6 h-full">
          {selectedUser && <UserDetails selectedUser={selectedUser} />}
        </div>
      </div>
      <ConfirmationDialog
        open={!!userToDelete}
        onCancel={handleCancelDeleteUser}
        onConfirm={handleConfirmDeleteUser}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </div>
  );
};

export { AdminUsers };
