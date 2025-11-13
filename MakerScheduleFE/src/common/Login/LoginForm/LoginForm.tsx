import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import {
  Button,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormHelperText,
  Paper,
} from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import type { UserLogin } from "@ms/types/auth.types";
import { useMutation } from "@tanstack/react-query";
import { login } from "@ms/api/authentication.api";

const loginInitialFormData = {
  // email: "admin@ms.com",
  // password: "Admin123!",

  email: "customer1@ms.com",
  password: "Password123!",
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

interface LoginFormProps {
  closeFormDialog: () => void;
  showForgotPassword: () => void;
  showWelcomeMessage?: boolean;
}

const LoginForm = ({
  closeFormDialog,
  showForgotPassword,
  showWelcomeMessage = true,
}: LoginFormProps) => {
  const { getValues, control, handleSubmit, reset, formState, watch } =
    useForm<loginFormData>({
      resolver: zodResolver(loginValidationSchema),
      defaultValues: loginInitialFormData,
    });

  const { mutate: doLogin, isError: loginError } = useMutation({
    mutationKey: ["login"],
    mutationFn: login,
    onSuccess: () => closeFormDialog(),
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

  const emailWatch = watch("email");
  const passwordWatch = watch("password");

  return (
    <form onSubmit={handleSubmit(submit)}>
      {showWelcomeMessage && <DialogTitle>Welcome back!</DialogTitle>}
      <DialogContent>
        <Paper elevation={0} className="flex flex-col gap-4 p-2">
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
            <Button onClick={showForgotPassword}>Forgot Password</Button>
          </div>
        </Paper>
        <DialogActions>
          <Button onClick={closeFormDialog}>Close</Button>
          <Button
            variant="contained"
            type="submit"
            disabled={!(emailWatch && passwordWatch && formState.isValid)}
          >
            Login
          </Button>
        </DialogActions>
      </DialogContent>
    </form>
  );
};

export { LoginForm };
