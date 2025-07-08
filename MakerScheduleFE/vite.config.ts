import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import tailwindcss from "@tailwindcss/vite";
import path from "path";
import fs from "fs";

export default defineConfig(({ mode }) => ({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      "@ms": path.resolve(process.cwd(), "src"),
      "@ms/*": path.resolve(process.cwd(), "src/*"),
    },
  },
  server:
    mode === "development"
      ? {
          https: {
            key: fs.readFileSync(
              path.resolve(__dirname, "../cert/localhost-key.pem")
            ),
            cert: fs.readFileSync(
              path.resolve(__dirname, "../cert/localhost.pem")
            ),
          },
        }
      : undefined,
}));
