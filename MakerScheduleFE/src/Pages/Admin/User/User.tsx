import { deleteEvent } from "@ms/api/event.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmatoin";
import { useAdminUsersData } from "@ms/hooks/useAdminUsersData";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useEffect, useState, type TransitionEvent } from "react";
import { AdminUsersTable } from "./Table/Table";
import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import type { DomainUser } from "@ms/types/domain-user.types";
import { UserDetails } from "./Details/UserDetails";

const Users = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [selectedUser, setSelectedUser] = useState<DomainUser | undefined>(
    undefined
  );
  const [userToDelete, setUserToDelete] = useState<DomainUser | undefined>();
  const [filteredUsers, setFilteredUsers] = useState<DomainUser[]>([]);

  const { users, appMetaData } = useAdminUsersData();
  console.log("appMetaData", appMetaData);
  const queryClient = useQueryClient();

  useEffect(() => {
    if (!users) return;
    setFilteredUsers(users);
  }, [users]);

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEvent,
    onSuccess: () => {
      if (!userToDelete) return;

      queryClient.setQueryData(
        ["users"],
        (oldUsers: AxiosResponse<DomainUser[]>) => {
          if (!oldUsers) return oldUsers;
          return {
            ...oldUsers,
            data: oldUsers.data.filter((user) => user.id !== userToDelete.id),
          };
        }
      );

      if (selectedUser?.id === userToDelete.id) {
        setSelectedUser(undefined);
      }

      setUserToDelete(undefined);
    },
  });

  const handleDrawerClose = (refreshData: boolean) => {
    setRefreshData(refreshData);
    setIsDrawerOpen(false);
  };

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
    if (!value) {
      setFilteredUsers(users || []);
      return;
    }

    const lowerCaseValue = value.toLowerCase();
    const filtered =
      users?.filter((user) => {
        return (
          user.firstName?.toLowerCase().includes(lowerCaseValue) ||
          user.lastName?.toLowerCase().includes(lowerCaseValue) ||
          user?.email?.toLowerCase().includes(lowerCaseValue)
        );
      }) || [];
    setFilteredUsers(filtered);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${isDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: isDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventsHeader onCreateEvent={handleUserCreate} />
        <TextSearch onSearch={handleSearch} />
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

export { Users };
