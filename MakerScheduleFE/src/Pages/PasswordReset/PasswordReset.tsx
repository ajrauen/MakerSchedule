import { zodResolver } from "@hookform/resolvers/zod";
import {
  resetPassword,
  validateResetPasswordToken,
} from "@ms/api/domain-user.api";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { Button, Paper } from "@mui/material";
import { useMutation } from "@tanstack/react-query";
import { useNavigate, useSearch } from "@tanstack/react-router";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";

interface PasswordResetFormData {
  password: string;
  confirmPassword: string;
}

const passwordResetFormData: PasswordResetFormData = {
  password: "",
  confirmPassword: "",
};

const passwordResetValidationSchema = z
  .object({
    password: z
      .string()
      .min(8, "Password must be at least 8 characters long")
      .max(100, "Password must be at most 100 characters long")
      .regex(/[a-z]/, "Password must contain at least one lowercase letter")
      .regex(/[A-Z]/, "Password must contain at least one uppercase letter")
      .regex(/[0-9]/, "Password must contain at least one number")
      .regex(/[\W_]/, "Password must contain at least one special character"),
    confirmPassword: z
      .string()
      .min(8)
      .max(100)
      .regex(/[a-z]/)
      .regex(/[A-Z]/)
      .regex(/[0-9]/)
      .regex(/[\W_]/),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
  });

const PasswordReset = () => {
  const navigate = useNavigate();
  const { userId, token } = useSearch({ strict: false });

  const { control, handleSubmit } = useForm<PasswordResetFormData>({
    resolver: zodResolver(passwordResetValidationSchema),
    defaultValues: passwordResetFormData,
  });

  const { mutate: validateResetToken } = useMutation({
    mutationFn: () => validateResetPasswordToken(userId, token),
    onError: () =>
      navigate({
        to: "/home",
      }),
  });

  const { mutate: doResetPassword } = useMutation({
    mutationFn: resetPassword,
  });

  useEffect(() => {
    if (token && userId) {
      validateResetToken();
    }
  }, [token, userId, validateResetToken]);

  const submit = (data: PasswordResetFormData) => {
    doResetPassword({
      userId: userId,
      token: token,
      newPassword: data.password,
    });
  };

  return (
    <Paper sx={{ p: 4, maxWidth: 400, margin: "0 auto" }}>
      <form onSubmit={handleSubmit(submit)}>
        <FormTextField
          label="Password"
          control={control}
          name="password"
          type="password"
        />
        <FormTextField
          label="Confirm Password"
          control={control}
          name="confirmPassword"
          type="password"
        />
        <div>
          <Button variant="contained" color="primary" type="submit">
            Reset Password
          </Button>
        </div>
      </form>
    </Paper>
  );
};

export { PasswordReset };
