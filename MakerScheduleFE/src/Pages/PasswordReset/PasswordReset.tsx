import { zodResolver } from "@hookform/resolvers/zod";
import { login } from "@ms/api/authentication.api";
import {
  resetPassword,
  validateResetPasswordToken,
} from "@ms/api/domain-user.api";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import type { UserLogin } from "@ms/types/auth.types";
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
  const { email, token } = useSearch({ strict: false });

  const { control, handleSubmit, getValues } = useForm<PasswordResetFormData>({
    resolver: zodResolver(passwordResetValidationSchema),
    defaultValues: passwordResetFormData,
  });

  const onResetPasswordSuccess = () => {
    const login: UserLogin = {
      email,
      password: getValues().password,
    };
    debugger;
    doLogin({
      creds: login,
    });
  };

  const { mutate: validateResetToken } = useMutation({
    mutationFn: () => validateResetPasswordToken(email, token),
  });

  const { mutate: doLogin } = useMutation({
    mutationKey: ["login"],
    mutationFn: login,
    onSuccess: () =>
      navigate({
        to: "/",
      }),
  });

  const { mutate: doResetPassword } = useMutation({
    mutationFn: resetPassword,
    onSuccess: onResetPasswordSuccess,
  });

  useEffect(() => {
    if (token && email) {
      validateResetToken();
    }
  }, [token, email, validateResetToken]);

  const submit = (data: PasswordResetFormData) => {
    doResetPassword({
      email: email,
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
