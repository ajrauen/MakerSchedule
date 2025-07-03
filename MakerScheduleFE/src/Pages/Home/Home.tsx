import { Login } from "@ms/Pages/Home/Login/Login";
import { Link as MuiLink } from "@mui/material";
import { Link } from "@tanstack/react-router";

const Home = () => {
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
            <li className="border-r-[1px] pr-2">
              <Login />
            </li>
            <li>
              <Login />
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
