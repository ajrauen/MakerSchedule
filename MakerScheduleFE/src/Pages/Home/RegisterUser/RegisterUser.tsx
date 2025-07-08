import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormHelperText, Paper } from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { FormDialog } from "@ms/Components/FormComponents/FormDialog";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import type { RegisterCustomerRequest } from "@ms/types/customer.types";
import { registerNewCustomerUser } from "@ms/api/customer.api";
import { login } from "@ms/api/authentication.api";
import type { AxiosError } from "axios";
import type { RequestError } from "@ms/types/request-error.types";

const registerInitialFormData: RegisterCustomerRequest = {
  email: "",
  password: "",
  firstName: "",
  lastName: "",
  phoneNumber: "",
  address: "",
  preferredContactMethod: "Phone",
  notes: "",
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
  notes: z.string().optional(),
});

const RegisterUser = () => {
  const [isRegisterFormOpen, setIsRegisterFormOpen] = useState(false);
  const [errorCode, setErrorCode] = useState<string | undefined>();

  const { control, handleSubmit, reset } = useForm<RegisterCustomerRequest>({
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
    mutationFn: registerNewCustomerUser,
    onSuccess: (_, variables) => {
      doLogin({
        creds: { email: variables.email, password: variables.password },
      });
    },
    onError: (error: AxiosError<RequestError>) => {
      setErrorCode(error.response?.data.code);
    },
  });

  const submit = (data: RegisterCustomerRequest) => {
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
          <FormTextField label="Email" control={control} name="email" />
          <FormTextField
            label="Password"
            control={control}
            name="password"
            type="password"
          />
          <FormTextField
            label="First Name"
            control={control}
            name="firstName"
          />
          <FormTextField label="Last Name" control={control} name="lastName" />
          <FormTextField
            label="Phone Number"
            control={control}
            name="phoneNumber"
          />
          <FormTextField label="Address" control={control} name="address" />
          <FormSelect
            label="Preferred Contact Method"
            control={control}
            name="preferredContactMethod"
            options={[
              { label: "Phone", value: "Phone" },
              { label: "Email", value: "Email" },
            ]}
          />
          <FormTextField label="Notes" control={control} name="notes" />

          {errorCode && (
            <FormHelperText error={true}>{errorCode}</FormHelperText>
          )}
        </Paper>
      </FormDialog>
    </>
  );
};

export { RegisterUser };
