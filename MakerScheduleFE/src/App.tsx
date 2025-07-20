import { RouterProvider } from "@tanstack/react-router";

import "./App.css";
import { getRouterTree } from "@ms/configs/router.config";
import { CircularProgress, createTheme, ThemeProvider } from "@mui/material";
import { useApplicationData } from "@ms/hooks/useApplicationData";

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

  const { isLoading } = useApplicationData();

  return (
    <ThemeProvider theme={theme}>
      {isLoading ? (
        <div className="flex">
          <CircularProgress size={"6rem"} className="mx-auto mt-[20%]" />
        </div>
      ) : (
        <RouterProvider router={getRouterTree()} />
      )}
    </ThemeProvider>
  );
};

export { App };
