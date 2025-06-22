import { logout, refreshToken } from "@ms/api/authentication.api";
import { Button } from "@mui/material";

const Home = () => {
  const handleRefreshToken = () => refreshToken();
  const handleLogout = () => logout();

  return (
    <div>
      <Button onClick={handleRefreshToken}>refreshToken</Button>
      <Button onClick={handleLogout}>logout</Button>
    </div>
  );
};

export { Home };
