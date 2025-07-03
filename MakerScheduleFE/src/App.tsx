import { RouterProvider } from "@tanstack/react-router";

import "./App.css";
import { getRouterTree } from "@ms/configs/router.config";
import { useAppBootStrap } from "@ms/hooks/useAppBootStrap";
import { createTheme, ThemeProvider } from "@mui/material";

declare module "@mui/material/styles" {
  interface Palette {
    ochre: Palette["primary"];
  }

  interface PaletteOptions {
    ochre?: PaletteOptions["primary"];
  }
}

// Update the Button's color options to include an ochre option
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

  useAppBootStrap();

  return (
    <ThemeProvider theme={theme}>
      <RouterProvider router={getRouterTree()} />
    </ThemeProvider>
  );
};

export { App };
