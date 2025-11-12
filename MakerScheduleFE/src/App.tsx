import { RouterProvider } from "@tanstack/react-router";

import "./App.css";
import { getRouterTree } from "@ms/configs/router.config";
import { CircularProgress, createTheme, ThemeProvider } from "@mui/material";
import { Provider } from "react-redux";
import { store } from "@ms/redux/store";

declare module "@mui/material/styles" {
  interface Palette {
    ochre: Palette["primary"];
  }

  interface PaletteOptions {
    ochre?: PaletteOptions["primary"];
  }
}

declare module "@mui/material/Button" {
  interface ButtonPropsColorOverrides {
    ochre: true;
  }
}

const App = () => {
  const theme = createTheme({
    palette: {
      ochre: {
        main: "#E3D026",
        light: "#E9DB5D",
        dark: "#A29415",
        contrastText: "#242105",
      },
    },
  });

  const isLoading = false;

  return (
    <ThemeProvider theme={theme}>
      <Provider store={store}>
        {isLoading ? (
          <div className="flex">
            <CircularProgress size={"6rem"} className="mx-auto mt-[20%]" />
          </div>
        ) : (
          <RouterProvider router={getRouterTree()} />
        )}
      </Provider>
    </ThemeProvider>
  );
};

export { App };
