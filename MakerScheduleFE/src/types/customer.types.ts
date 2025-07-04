interface RegisterCustomerRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  preferredContactMethod: "Phone" | "Email";
  notes?: string;
}

export { type RegisterCustomerRequest };
