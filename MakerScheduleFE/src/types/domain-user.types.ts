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
  id: string;
  firstName: string;
  lastName: string;
}

export { type RegisterDomainUserRequest, type DomainUser };
