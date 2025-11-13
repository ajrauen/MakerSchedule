import { useState } from "react";

import { Dialog } from "@mui/material";
import { Login } from "@ms/common/Login/Login";

const LoginHeader = () => {
  const [isLoginFormOpen, setIsLoginFormOpen] = useState(false);

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
        <Login
          isOpen={isLoginFormOpen}
          onClose={() => setIsLoginFormOpen(false)}
        />
      </Dialog>
    </>
  );
};

export { LoginHeader };
