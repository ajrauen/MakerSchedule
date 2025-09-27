import { Login } from "@ms/Pages/Home/Login/Login";
import { RegisterUser } from "@ms/Pages/Home/RegisterUser/RegisterUser";
import { Link } from "@tanstack/react-router";
import { refreshToken } from "@ms/api/authentication.api";
import { useEffect } from "react";
import { removeToken } from "@ms/utils/auth.utils";
import { useQuery } from "@tanstack/react-query";
import { useIsLoggedIn } from "@ms/hooks/useIsLoggedIn";

const Home = () => {
  const isLoggedIn = useIsLoggedIn();

  const { error: isError } = useQuery({
    queryKey: ["refreshTokenOnLoad"],
    queryFn: refreshToken,
    enabled: !isLoggedIn,
    retry: false,
    throwOnError: false,
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
        <div className=" h-full relative content-center text-center mx-auto ">
          <h2 className="!text-white text-5xl">
            Empowering Creativity through Controlled Chaos
          </h2>
          <div className="flex flex-col gap-4 items-center mt-12 md:flex-row md:gap-8 md:justify-center">
            <Link
              to="/spaces"
              className="bg-amber-200 px-4 text-3xl py-3 font-medium uppercase rounded-lg cursor-pointer hover:bg-green-400 hover:text-white w-fit"
            >
              Our Spaces
            </Link>
            <Link
              to="/schedule-tour"
              className="bg-amber-200 px-4 text-3xl py-3 font-medium uppercase rounded-lg cursor-pointer hover:bg-green-400 hover:text-white w-fit"
            >
              Schedule a tour
            </Link>
          </div>
        </div>

        <div className="bg-white/90 backdrop-blur-sm mx-4 md:mx-8 lg:mx-16 rounded-lg  mt-8">
          <section className="p-8 text-center">
            <h3 className="text-4xl font-bold text-gray-800 mb-6">
              Why Become a Member?
            </h3>
            <p className="text-lg text-gray-700 mb-6 max-w-3xl mx-auto">
              Join our vibrant community of makers and creators! With 24/7
              access to professional-grade tools, expert-led workshops, and a
              collaborative environment, you'll have everything you need to
              bring your creative visions to life. From beginners to seasoned
              professionals, our space empowers every maker to explore, learn,
              and create without limits.
            </p>
            <Link
              to="/membership"
              className="bg-green-500 px-6 text-2xl py-3 font-medium uppercase rounded-lg cursor-pointer hover:bg-green-600 hover:text-white text-white w-fit inline-block transition-colors"
            >
              Become a Member
            </Link>
          </section>

          <section className="p-8 bg-gray-50 rounded-lg mx-4 mb-8">
            <h3 className="text-4xl font-bold text-gray-800 mb-4 text-center">
              Explore Our Classes
            </h3>
            <p className="text-lg text-gray-700 mb-8 text-center max-w-2xl mx-auto">
              From woodworking to 3D printing, we offer hands-on classes for all
              skill levels. Learn new techniques, master professional tools, and
              create alongside fellow makers.
            </p>

            <div className="text-center mb-6">
              <h4 className="text-2xl font-semibold text-gray-800 mb-6">
                Explore Our Classes
              </h4>
              <div className="grid grid-cols-2 md:grid-cols-4 gap-6 max-w-4xl mx-auto">
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🪵</div>
                  <span className="text-sm font-medium text-gray-700">
                    Woodworking
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🖨️</div>
                  <span className="text-sm font-medium text-gray-700">
                    3D Printing
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">⚡</div>
                  <span className="text-sm font-medium text-gray-700">
                    Electronics
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🔧</div>
                  <span className="text-sm font-medium text-gray-700">
                    Metalworking
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🧵</div>
                  <span className="text-sm font-medium text-gray-700">
                    Textiles
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🎨</div>
                  <span className="text-sm font-medium text-gray-700">
                    Arts & Crafts
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">💎</div>
                  <span className="text-sm font-medium text-gray-700">
                    Jewelry
                  </span>
                </div>
                <div className="flex flex-col items-center p-4 bg-white rounded-lg shadow hover:shadow-md transition-shadow cursor-pointer">
                  <div className="text-4xl mb-2">🏺</div>
                  <span className="text-sm font-medium text-gray-700">
                    Ceramics
                  </span>
                </div>
              </div>
            </div>
          </section>

          <section className="p-8 text-center">
            <h3 className="text-4xl font-bold text-gray-800 mb-6">
              About Seeded Chaos
            </h3>
            <div className="max-w-4xl mx-auto text-lg text-gray-700 space-y-4">
              <p>
                Founded on the principle that creativity thrives in controlled
                chaos, Seeded Chaos is a premier maker education center
                dedicated to teaching the art and craft of creation. We offer
                hands-on classes across diverse making disciplines, from
                traditional woodworking and ceramics to cutting-edge 3D printing
                and electronics.
              </p>
              <p>
                Our expert instructors guide students of all skill levels
                through structured learning experiences, providing both the
                technical knowledge and creative confidence needed to master new
                making skills. Each class is designed to be engaging,
                educational, and inspiring—whether you're taking your first
                steps in a craft or looking to refine advanced techniques.
              </p>
              <p>
                At Seeded Chaos, we believe learning should be collaborative and
                fun. Our welcoming community of students and instructors creates
                an environment where curiosity is celebrated, mistakes become
                learning opportunities, and every completed project is a step
                forward in your maker journey.
              </p>
            </div>
          </section>
        </div>
      </div>

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
            <Link to={"profile"}>
              <li>Profile</li>
            </Link>
          </ul>
          <div>
            {/* <Link to={"classes"}> */}
            {/* <MuiLink>Class Scheduel</MuiLink>
            </Link> */}
          </div>
        </div>
      </div>
    </div>
  );
};

export { Home };
