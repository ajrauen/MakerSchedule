import { useState } from "react";
import { LoginForm } from "@ms/Pages/Home/Login/LoginForm/LoginForm";
import { ForgotPasswordForm } from "@ms/Pages/Home/Login/ForgotPasswordForm/ForgotPasswordForm";
import { Dialog } from "@mui/material";

const Login = () => {
  const [isLoginFormOpen, setIsLoginFormOpen] = useState(false);
  const [isForgotPasswordView, setIsForgotPasswordView] = useState(false);

  return (
    <>
      <span className="cursor-pointer" onClick={() => setIsLoginFormOpen(true)}>
        Login
      </span>
      <Dialog
        open={isLoginFormOpen}
        onClose={() => setIsLoginFormOpen(false)}
        fullWidth
      >
        {isForgotPasswordView ? (
          <ForgotPasswordForm
            closeFormDialog={() => {
              setIsForgotPasswordView(false);
            }}
            isOpen={isForgotPasswordView}
          />
        ) : (
          <LoginForm
            closeFormDialog={() => setIsLoginFormOpen(false)}
            isOpen={isLoginFormOpen}
            showForgotPassword={() => setIsForgotPasswordView(true)}
          />
        )}
      </Dialog>
    </>
  );
};

export { Login };
