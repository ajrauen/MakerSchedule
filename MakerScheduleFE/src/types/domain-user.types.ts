interface RegisterDomainUserRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  preferredContactMethod: "Phone" | "Email";
}

type UpdateDomainUserRequest = Partial<
  Partial<Omit<RegisterDomainUserRequest, "password">>
>;

interface UpdateUserPassword {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

interface DomainUser {
  firstName?: string;
  lastName?: string;
  id: string;
  roles?: string[];
  email?: string;
}

interface UserMetaData {
  roles: string[];
}

interface DomainUserRegisteredEvent {
  attended: boolean;
  description?: string;
  eventId: string;
  eventName: string;
  occurrenceId: string;
  occurrenceStartTime: string;
  scheduleEnd: string;
  registeredAt: string;
  duration: number;
}

export {
  type RegisterDomainUserRequest,
  type UpdateDomainUserRequest,
  type DomainUser,
  type UserMetaData,
  type UpdateUserPassword,
  type DomainUserRegisteredEvent,
};
