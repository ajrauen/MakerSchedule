import { z } from "zod/v4";

// Password validation - reusable
const passwordValidation = z
  .string()
  .min(7, { message: "Password must be at least 7 characters" })
  .regex(/[A-Z]/, { message: "Password must contain an uppercase letter" })
  .regex(/[a-z]/, { message: "Password must contain a lowercase letter" })
  .regex(/[0-9]/, { message: "Password must contain a number" })
  .regex(/[^A-Za-z0-9]/, {
    message: "Password must contain a special character",
  });

const userInfoValidationSchema = z.object({
  email: z.email(),
  password: passwordValidation,
  firstName: z.string().min(1, { message: "First name is required" }),
  lastName: z.string().min(1, { message: "Last name is required" }),
  phoneNumber: z.string().min(7, { message: "Phone number is required" }),
  address: z.string().min(1, { message: "Address is required" }),
  preferredContactMethod: z.enum(["Phone", "Email"], {
    message: "Preferred contact method is required",
  }),
});

const updateUserInfoValidationSchema = z
  .object({
    email: z.string().optional(),
    firstName: z.string().optional(),
    lastName: z.string().optional(),
    phoneNumber: z.string().optional(),
    address: z.string().optional(),
    preferredContactMethod: z.enum(["Phone", "Email"]).optional(),
  })
  .refine(
    (data) => {
      if (data.email && data.email.trim() !== "") {
        return z.email().safeParse(data.email).success;
      }
      return true;
    },
    { message: "Invalid email format", path: ["email"] }
  )
  .refine(
    (data) => {
      if (data.firstName && data.firstName.trim() !== "") {
        return data.firstName.trim().length >= 1;
      }
      return true;
    },
    { message: "First name is required", path: ["firstName"] }
  )
  .refine(
    (data) => {
      if (data.lastName && data.lastName.trim() !== "") {
        return data.lastName.trim().length >= 1;
      }
      return true;
    },
    { message: "Last name is required", path: ["lastName"] }
  )
  .refine(
    (data) => {
      if (data.phoneNumber && data.phoneNumber.trim() !== "") {
        return data.phoneNumber.trim().length >= 7;
      }
      return true;
    },
    {
      message: "Phone number must be at least 7 characters",
      path: ["phoneNumber"],
    }
  )
  .refine(
    (data) => {
      if (data.address && data.address.trim() !== "") {
        return data.address.trim().length >= 1;
      }
      return true;
    },
    { message: "Address is required", path: ["address"] }
  );

const updateUserPasswordValidationSchema = z
  .object({
    currentPassword: passwordValidation,
    newPassword: passwordValidation,
    confirmPassword: passwordValidation,
  })
  .refine((data) => data.newPassword.trim() === data.confirmPassword.trim(), {
    message: "Passwords don't match",
    path: ["confirmPassword"],
  })
  .refine((data) => data.currentPassword.trim() !== data.newPassword.trim(), {
    message: "New password must be different from current password",
    path: ["newPassword"],
  });

export {
  userInfoValidationSchema,
  updateUserInfoValidationSchema,
  updateUserPasswordValidationSchema,
};
