import { ForgotPasswordForm } from "@ms/common/Login/ForgotPasswordForm/ForgotPasswordForm";
import { LoginForm } from "@ms/common/Login/LoginForm/LoginForm";
import { useState } from "react";

interface LoginDialogProps {
  onClose: () => void;
  showWelcomeMessage?: boolean;
}

const Login = ({ onClose, showWelcomeMessage = true }: LoginDialogProps) => {
  const [isForgotPasswordView, setIsForgotPasswordView] = useState(false);

  return isForgotPasswordView ? (
    <ForgotPasswordForm
      closeFormDialog={() => {
        setIsForgotPasswordView(false);
      }}
      isOpen={isForgotPasswordView}
    />
  ) : (
    <LoginForm
      closeFormDialog={onClose}
      showForgotPassword={() => setIsForgotPasswordView(true)}
      showWelcomeMessage={showWelcomeMessage}
    />
  );
};

export { Login };
