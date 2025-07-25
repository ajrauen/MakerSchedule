interface RegisterDomainUserRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  preferredContactMethod: "Phone" | "Email";
}

interface DomainUser {
  firstName?: string;
  lastName?: string;
  id: string;
  roles?: string[];
  email?: string;
}

export { type RegisterDomainUserRequest, type DomainUser };
