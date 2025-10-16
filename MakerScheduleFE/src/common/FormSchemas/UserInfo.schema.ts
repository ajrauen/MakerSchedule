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

// Helper to treat empty strings as undefined for partial updates
const emptyStringToUndefined = <T extends z.ZodTypeAny>(schema: T) =>
  z.preprocess((val) => (val === "" ? undefined : val), schema);

const updateUserInfoValidationSchema = z.object({
  email: emptyStringToUndefined(z.email().optional()),
  firstName: emptyStringToUndefined(
    z.string().min(1, { message: "First name is required" }).optional()
  ),
  lastName: emptyStringToUndefined(
    z.string().min(1, { message: "Last name is required" }).optional()
  ),
  phoneNumber: emptyStringToUndefined(
    z.string().min(7, { message: "Phone number is required" }).optional()
  ),
  address: emptyStringToUndefined(
    z.string().min(1, { message: "Address is required" }).optional()
  ),
  preferredContactMethod: emptyStringToUndefined(
    z
      .enum(["Phone", "Email"], {
        message: "Preferred contact method is required",
      })
      .optional()
  ),
});

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
