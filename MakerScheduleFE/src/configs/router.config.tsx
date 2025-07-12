import { CreateEvent } from "@ms/Components/CreateEvent/CreateEvent";
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
  // Create the root route
  const rootRoute = createRootRoute({
    component: () => (
      <>
        <Outlet />
        <TanStackRouterDevtools />
      </>
    ),
  });

  // Create child routes
  const homeRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/",
    component: Home,
  });
  const aboutRoute = createRoute({
    getParentRoute: () => rootRoute,
    path: "/about",
    component: CreateEvent,
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

  // Build the route tree
  const routeTree = rootRoute.addChildren([
    homeRoute,
    aboutRoute,
    ClassRoute,
    AdminRoute,
  ]);

  // Create the router
  const router = createRouter({
    routeTree,
  });

  return router;
};

export { getRouterTree };
