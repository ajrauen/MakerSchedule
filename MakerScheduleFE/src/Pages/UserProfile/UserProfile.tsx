import { getActiveUser } from "@ms/api/domain-user.api";
import { useIsLoggedIn } from "@ms/hooks/useIsLoggedIn";
import { PasswordChange } from "@ms/Pages/UserProfile/PasswordChange/PasswordChange";
import { UserInfo } from "@ms/Pages/UserProfile/UserInfo.tsx/UserInfo";
import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "@tanstack/react-router";

const UserProfile = () => {
  const isLoggedIn = useIsLoggedIn();
  const navigate = useNavigate();

  if (!isLoggedIn) {
    navigate({ to: "/" });
  }

  const { data: userData } = useQuery({
    queryKey: ["userInfo"],
    queryFn: getActiveUser,
    enabled: isLoggedIn,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });

  return (
    <div className="flex flex-col h-full overflow-hidden p-4 items-center gap-4">
      <UserInfo userData={userData} />
      <PasswordChange userData={userData} />
    </div>
  );
};

export { UserProfile };
