import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { Button, FormHelperText, Paper } from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import type { UserLogin } from "@ms/types/auth.types";
import { useMutation } from "@tanstack/react-query";
import { login } from "@ms/api/authentication.api";
import { useState } from "react";
import { FormDialog } from "@ms/Components/FormComponents/FormDialog";
import { sendPasswordResetRequest } from "@ms/api/domain-user.api";

const loginInitialFormData = {
  email: "a@b.com",
  password: "Tex@s123",
};

const loginValidationSchema = z.object({
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

type loginFormData = z.infer<typeof loginValidationSchema>;

const Login = () => {
  const [isLoginFormOpen, setIsLoginFormOpen] = useState(false);

  const { getValues, control, handleSubmit, reset } = useForm<loginFormData>({
    resolver: zodResolver(loginValidationSchema),
    defaultValues: loginInitialFormData,
  });

  const { mutate: doLogin, isError: loginError } = useMutation({
    mutationKey: ["login"],
    mutationFn: login,
    onSuccess: () => setIsLoginFormOpen(false),
  });

  const { mutate: doSendPasswordResetRequest } = useMutation({
    mutationKey: ["sendPasswordResetRequest"],
    mutationFn: sendPasswordResetRequest,
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

  // const loginButtonDisabled = useMemo(() => {
  //   const { email, password } = getValues();
  //   return email !== "" && password !== "";
  // }, [formState]);

  const openLoginForm = () => setIsLoginFormOpen(true);
  const closeLoginForm = () => setIsLoginFormOpen(false);

  const handeForgotPassword = () => {
    const { email } = getValues();
    if (email && email !== "") {
      doSendPasswordResetRequest(email);
    }
  };

  return (
    <>
      <span className="cursor-pointer" onClick={openLoginForm}>
        Login
      </span>
      <FormDialog
        open={isLoginFormOpen}
        onSubmit={handleSubmit(submit)}
        onClose={closeLoginForm}
        submitText="Login"
        maxWidth="sm"
        fullWidth
      >
        <Paper elevation={0} className="flex flex-col gap-4 p-2">
          <h3 className="text-purple-300 text-2xl">Welcome back!</h3>
          <span className="text-3xl mb-4">Login to your account</span>
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

          <div className="flex items-center w-full">
            <div className="flex-1 h-px bg-gray-300 relative top-[3px]" />
            <span className="mx-2 text-gray-300 text-xs  leading-0.5">or</span>
            <div className="flex-1 h-px bg-gray-300 relative top-[3px]" />
          </div>
          <div className="flex items-center align-middle mx-auto w-full">
            <Button onClick={handeForgotPassword}>Forgot Password</Button>
          </div>
        </Paper>
      </FormDialog>
    </>
  );
};

export { Login };
