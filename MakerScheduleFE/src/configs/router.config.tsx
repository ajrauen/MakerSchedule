import { Admin } from "@ms/Pages/Admin/Admin";
import { Classes } from "@ms/Pages/Classes/Classes";
import { Home } from "@ms/Pages/Home/Home";
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

  const routeTree = rootRoute.addChildren([homeRoute, ClassRoute, AdminRoute]);

  const router = createRouter({
    routeTree,
  });

  return router;
};

export { getRouterTree };
