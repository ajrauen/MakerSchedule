import { Login } from "@ms/Pages/Home/Login/Login";
import { RegisterUser } from "@ms/Pages/Home/RegisterUser/RegisterUser";
import { useIsLoggedIn } from "@ms/utils/auth.utils";
import { Link as MuiLink } from "@mui/material";
import { Link } from "@tanstack/react-router";
import { refreshToken } from "@ms/api/authentication.api";
import { useEffect } from "react";
import { removeToken } from "@ms/utils/auth.utils";
import { useQuery } from "@tanstack/react-query";

const Home = () => {
  const isLoggedIn = useIsLoggedIn();

  const { error, isError } = useQuery({
    queryKey: ["refreshTokenOnLoad"],
    queryFn: refreshToken,
    enabled: isLoggedIn,
    retry: false,
  });

  useEffect(() => {
    if (isError) {
      removeToken();
    }
  }, [isError]);

  return (
    <div className="w-full h-full ">
      <div className="w-full h-full absolute">
        <video
          autoPlay
          muted
          loop
          className="w-full h-full absolute object-cover"
        >
          <source src="/homeVideo.mp4" type="video/mp4" />
        </video>
      </div>
      <div className="grid grid-cols-3  w-full px-2 pb-2 bg-purple-300/30 absolute top-0 left-0 z-1000">
        <div>Place Holder</div>
        <div className="justify-self-center flex flex-col items-center">
          <h1 className="text-white">Seeded Chaos</h1>
          <div className="flex flex-row gap-3">
            <Link to={"classes"}>
              <MuiLink>Class Scheduel</MuiLink>
            </Link>
            <Link to={"asdfsaf"}>
              <MuiLink>Class Scheduel</MuiLink>
            </Link>
            <Link to={"asdfsaf"}>
              <MuiLink>Class Scheduel</MuiLink>
            </Link>
          </div>
        </div>
        <div className="justify-self-end text-white">
          <ul className="flex gap-2">
            {!isLoggedIn && (
              <li className="border-r-[1px] pr-2">
                <Login />
              </li>
            )}

            <li>
              <RegisterUser />
            </li>
          </ul>
          <div>
            <Link to={"classes"}>
              <MuiLink>Class Scheduel</MuiLink>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export { Home };
