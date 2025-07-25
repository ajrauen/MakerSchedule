import { Paper } from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { FormDialog } from "@ms/Components/FormComponents/FormDialog";
import type { RegisterDomainUserRequest } from "@ms/types/domain-user.types";
import { login } from "@ms/api/authentication.api";
import type { AxiosError } from "axios";
import type { RequestError } from "@ms/types/request-error.types";
import { registerNewDomainUser } from "@ms/api/domain-user.api";
import { UserForm } from "@ms/Components/UserForm/UserForm";

const registerInitialFormData: RegisterDomainUserRequest = {
  email: "",
  password: "",
  firstName: "",
  lastName: "",
  phoneNumber: "",
  address: "",
  preferredContactMethod: "Phone",
};

const registerValidationSchema = z.object({
  email: z.string().email(),
  password: z
    .string()
    .min(7, { message: "Password must be at least 7 characters" })
    .regex(/[A-Z]/, { message: "Password must contain an uppercase letter" })
    .regex(/[a-z]/, { message: "Password must contain a lowercase letter" })
    .regex(/[0-9]/, { message: "Password must contain a number" })
    .regex(/[^A-Za-z0-9]/, {
      message: "Password must contain a special character",
    }),
  firstName: z.string().min(1, { message: "First name is required" }),
  lastName: z.string().min(1, { message: "Last name is required" }),
  phoneNumber: z.string().min(7, { message: "Phone number is required" }),
  address: z.string().min(1, { message: "Address is required" }),
  preferredContactMethod: z.enum(["Phone", "Email"], {
    message: "Preferred contact method is required",
  }),
});

const RegisterUser = () => {
  const [isRegisterFormOpen, setIsRegisterFormOpen] = useState(false);
  const [errorCode, setErrorCode] = useState<string | undefined>();

  const { control, handleSubmit, reset } = useForm<RegisterDomainUserRequest>({
    resolver: zodResolver(registerValidationSchema),
    defaultValues: registerInitialFormData,
  });

  const { mutate: doLogin } = useMutation({
    mutationKey: ["login"],
    mutationFn: login,
    onSuccess: () => {
      setIsRegisterFormOpen(false);
    },
    onError: () => {
      setIsRegisterFormOpen(false);
    },
  });

  const { mutate: doRegister } = useMutation({
    mutationKey: ["registerUser"],
    mutationFn: registerNewDomainUser,
    onSuccess: (_, variables) => {
      doLogin({
        creds: { email: variables.email, password: variables.password },
      });
    },
    onError: (error: AxiosError<RequestError>) => {
      setErrorCode(error.response?.data.code);
    },
  });

  const submit = (data: RegisterDomainUserRequest) => {
    doRegister(data);
    reset(undefined, { keepValues: true });
  };

  const openRegisterForm = () => setIsRegisterFormOpen(true);
  const closeRegisterForm = () => setIsRegisterFormOpen(false);

  return (
    <>
      <span className="cursor-pointer" onClick={openRegisterForm}>
        Register
      </span>
      <FormDialog
        open={isRegisterFormOpen}
        onSubmit={handleSubmit(submit)}
        onClose={closeRegisterForm}
        submitText="Register"
        maxWidth="sm"
        fullWidth
      >
        <Paper elevation={0} className="flex flex-col gap-4 p-2">
          <h3 className="text-purple-300 text-2xl">Create an account</h3>
          <span className="text-3xl mb-4">Register a new user</span>
          <UserForm control={control} errorCode={errorCode} />
        </Paper>
      </FormDialog>
    </>
  );
};

export { RegisterUser };
