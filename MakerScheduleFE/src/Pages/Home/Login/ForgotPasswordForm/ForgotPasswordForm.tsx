import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import {
  Button,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
} from "@mui/material";
import { useForm } from "react-hook-form";
import { z } from "zod/v4";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
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

interface ForgotPasswordFormProps {
  closeFormDialog: () => void;
  isOpen: boolean;
}

const ForgotPasswordForm = ({ closeFormDialog }: ForgotPasswordFormProps) => {
  const { getValues, control, handleSubmit, formState, watch } =
    useForm<loginFormData>({
      resolver: zodResolver(loginValidationSchema),
      defaultValues: loginInitialFormData,
    });

  const { mutate: doSendPasswordResetRequest, isSuccess } = useMutation({
    mutationKey: ["sendPasswordResetRequest"],
    mutationFn: sendPasswordResetRequest,
    meta: {
      errorMessage: "Unable to send password reset request. Please try again.",
    },
  });

  const submit = () => {
    const { email } = getValues();
    if (email && email !== "") {
      doSendPasswordResetRequest(email);
    }
  };

  const emailWatch = watch("email");
  const passwordWatch = watch("password");

  return (
    <form onSubmit={handleSubmit(submit)}>
      <DialogTitle>ForgotPassword</DialogTitle>
      <DialogContent>
        <Paper elevation={0} className="flex flex-col gap-4 p-2">
          {isSuccess ? (
            <div>
              If there is an email associated with this account, you will
              receive a password reset link in your email.
            </div>
          ) : (
            <FormTextField label="Email" control={control} name="email" />
          )}
        </Paper>
        <DialogActions>
          <Button onClick={closeFormDialog}>Close</Button>
          <Button
            variant="contained"
            type="submit"
            disabled={!(emailWatch && passwordWatch && formState.isValid)}
          >
            Submit
          </Button>
        </DialogActions>
      </DialogContent>
    </form>
  );
};

export { ForgotPasswordForm };
