const { VITE_SERVER_URI: SERVER_URI } = import.meta.env as Record<
  string,
  string
>;
console.log(import.meta.env.VITE_SERVER_URI);
export { SERVER_URI };
