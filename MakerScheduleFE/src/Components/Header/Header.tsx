import { getActiveUser } from "@ms/api/domain-user.api";
import { useIsLoggedIn } from "@ms/hooks/useIsLoggedIn";
import { useRefreshTokenOnLoad } from "@ms/hooks/useRefreshTokenOnLoad";
import { Login } from "@ms/Pages/Home/Login/Login";
import { RegisterUser } from "@ms/Pages/Home/RegisterUser/RegisterUser";
import { useQuery } from "@tanstack/react-query";
import { Link } from "@tanstack/react-router";

const Header = () => {
  const isLoggedIn = useIsLoggedIn();
  useRefreshTokenOnLoad();

  const { data: userInfo } = useQuery({
    queryKey: ["userInfo"],
    queryFn: getActiveUser,
    enabled: isLoggedIn,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });

  return (
    <div className="grid grid-cols-3  w-full p-4 bg-purple-300/30 absolute top-0 left-0 z-1000">
      <div>Place Holder</div>
      <div className="justify-self-center flex flex-col items-center">
        <h5 className="!text-white text-3xl">Seeded Chaos</h5>
        <div className="flex flex-row gap-3"></div>
      </div>
      <div className="justify-self-end text-white">
        <ul className="flex gap-2">
          {!isLoggedIn && (
            <>
              <li className="border-r-[1px] pr-2">
                <Login />
              </li>
              <li>
                <RegisterUser />
              </li>
            </>
          )}

          {isLoggedIn && (
            <Link to={"profile"}>
              <li>Profile</li>
            </Link>
          )}
          {userInfo?.roles?.includes("Admin") && (
            <Link to={"admin"}>
              <li>Admin</li>
            </Link>
          )}
        </ul>
        <div>
          {/* <Link to={"classes"}> */}
          {/* <MuiLink>Class Scheduel</MuiLink>
            </Link> */}
        </div>
      </div>
    </div>
  );
};

export { Header };
