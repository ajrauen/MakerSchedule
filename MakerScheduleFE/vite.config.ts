import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import tailwindcss from "@tailwindcss/vite";
import path from "path";
import fs from "fs";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      "@ms": path.resolve(process.cwd(), "src"),
      "@ms/*": path.resolve(process.cwd(), "src/*"),
    },
  },
  server: {
    https: {
      key: fs.readFileSync(
        path.resolve(__dirname, "../cert/localhost-key.pem")
      ),
      cert: fs.readFileSync(path.resolve(__dirname, "../cert/localhost.pem")),
    },
  },
});
