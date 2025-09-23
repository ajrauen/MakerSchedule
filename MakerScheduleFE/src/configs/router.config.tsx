import { Admin } from "@ms/Pages/Admin/Admin";
import { Classes } from "@ms/Pages/Classes/Classes";
import { Home } from "@ms/Pages/Home/Home";
import { PasswordReset } from "@ms/Pages/PasswordReset/PasswordReset";
import { UserProfile } from "@ms/Pages/UserProfile/UserProfile";
import {
  createRootRoute,
  createRoute,
  createRouter,
  Outlet,
} from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

const getRouterTree = () => {
  const rootRoute = createRootRoute({
    component: () => (
      <>
        <Outlet />
        <TanStackRouterDevtools />
      </>
    ),
  });

  const homeRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/",
    component: Home,
  });

  const ClassRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/classes",
    component: Classes,
  });

  const AdminRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/admin",
    component: Admin,
  });

  const UserProfileRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/profile",
    component: UserProfile,
  });

  const PasswordResetRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "password-reset",
    component: PasswordReset,
  });

  const routeTree = rootRoute.addChildren([
    homeRoute,
    ClassRoute,
    AdminRoute,
    UserProfileRoute,
    PasswordResetRoute,
  ]);

  const router = createRouter({
    routeTree,
  });

  return router;
};

export { getRouterTree };
