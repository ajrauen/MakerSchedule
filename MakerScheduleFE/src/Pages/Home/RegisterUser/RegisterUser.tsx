import { Paper } from "@mui/material";
import { FormProvider, useForm } from "react-hook-form";
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
import { userInfoValidationSchema } from "@ms/common/FormSchemas/UserInfo.schema";

const registerInitialFormData: RegisterDomainUserRequest = {
  email: "",
  password: "",
  firstName: "",
  lastName: "",
  phoneNumber: "",
  address: "",
  preferredContactMethod: "Phone",
};

const RegisterUser = () => {
  const [isRegisterFormOpen, setIsRegisterFormOpen] = useState(false);
  const [errorCode, setErrorCode] = useState<string | undefined>();

  const hookForm = useForm<RegisterDomainUserRequest>({
    resolver: zodResolver(userInfoValidationSchema),
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
    hookForm.reset(undefined, { keepValues: true });
  };

  const openRegisterForm = () => setIsRegisterFormOpen(true);
  const closeRegisterForm = () => setIsRegisterFormOpen(false);

  return (
    <>
      <FormProvider {...hookForm}>
        <span className="cursor-pointer" onClick={openRegisterForm}>
          Register
        </span>
        <FormDialog
          open={isRegisterFormOpen}
          onSubmit={hookForm.handleSubmit(submit)}
          onClose={closeRegisterForm}
          submitText="Register"
          maxWidth="sm"
          fullWidth
        >
          <Paper elevation={0} className="flex flex-col gap-4 p-2">
            <h3 className="text-purple-300 text-2xl">Create an account</h3>
            <span className="text-3xl mb-4">Register a new user</span>
            <UserForm errorCode={errorCode} />
          </Paper>
        </FormDialog>
      </FormProvider>
    </>
  );
};

export { RegisterUser };
