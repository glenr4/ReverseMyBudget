import passTokenToApi, { signIn, signOut } from "./auth/auth";

const App = () => {
  return (
    <div>
      <button onClick={signIn}>Sign In</button>
      <button onClick={signOut}>Sign Out</button>
      <button onClick={passTokenToApi}>Call API</button>
    </div>
  );
};
export default App;
