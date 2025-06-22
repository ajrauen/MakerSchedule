import FormTextField from "@ms/Components/FormTextField/FormTextField";
import { Button, FormHelperText, Paper } from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import type { UserLogin } from "@ms/types/auth.types";
import { useMutation } from "@tanstack/react-query";
import { login } from "@ms/api/authentication.api";
import { useNavigate } from "@tanstack/react-router";
import { useMemo } from "react";

interface FormData {
  email: string;
  password: string;
}

const Login = () => {
  const navigate = useNavigate();

  const initialFormData = {
    email: "a@b.com",
    password: "Tex@s123",
  };

  const validationSchema = z.object({
    email: z.email(),
    password: z
      .string()
      .min(7, { message: "Password must be at least 7 characters" })
      .regex(/[A-Z]/, { message: "Password must contain an uppercase letter" })
      .regex(/[a-z]/, { message: "Password must contain a lowercase letter" })
      .regex(/[0-9]/, { message: "Password must contain a number" })
      .regex(/[^A-Za-z0-9]/, {
        message: "Password must contain a special character",
      }),
  });

  const { getValues, control, handleSubmit, reset, formState } =
    useForm<FormData>({
      resolver: zodResolver(validationSchema),
      defaultValues: initialFormData,
    });

  const {
    mutate: doLogin,
    isError: loginError,
    isPending: isPendingLogin,
  } = useMutation({
    mutationKey: ["login"],
    mutationFn: login,
    onSuccess: () => navigate({ to: "/" }),
  });

  const submit = () => {
    const { email, password } = getValues();
    const login: UserLogin = {
      email,
      password,
    };

    doLogin({
      creds: login,
    });
    reset(undefined, { keepValues: true });
  };

  const loginButtonDisabled = useMemo(() => {
    const { email, password } = getValues();
    return email !== "" && password !== "";
  }, [formState]);

  return (
    <div>
      <Paper elevation={3} className="flex flex-col gap-4 p-16">
        <FormTextField label="Email" control={control} name="email" />
        <FormTextField
          label="Password"
          control={control}
          name="password"
          type="password"
        />

        {loginError && (
          <FormHelperText error={true}>
            Username or Password is incorrect
          </FormHelperText>
        )}
        <Button
          variant="contained"
          onClick={handleSubmit(submit)}
          loading={isPendingLogin}
        >
          Login
        </Button>
      </Paper>
    </div>
  );
};

export { Login };
