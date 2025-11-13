import { useIsLoggedIn } from "@ms/hooks/useIsLoggedIn";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { Button } from "@mui/material";

interface ClassDescriptionProps {
  event: EventOffering;
  occurrence?: Occurrence;
  onShowRegistration: (show: boolean) => void;
  setShowLoginView: (show: boolean) => void;
}

const ClassDescription = ({
  event,
  occurrence,
  onShowRegistration,
  setShowLoginView,
}: ClassDescriptionProps) => {
  const isLoggedIn = useIsLoggedIn();

  const handleActionClick = () => {
    if (isLoggedIn) {
      onShowRegistration(true);
    } else {
      setShowLoginView(true);
    }
  };

  return (
    <div>
      <h2 className="text-lg font-semibold mb-2">Description</h2>
      <p>{event.description}</p>
      <hr className="my-3 text-gray-300" />
      <span className="flex">
        <strong>Price</strong>
        <div>
          <strong>Open Slots:</strong> 3
        </div>
      </span>
      <hr className="my-3 text-gray-300" />
      <div>
        {occurrence ? (
          <Button variant="contained" size="small" onClick={handleActionClick}>
            {isLoggedIn ? "Register" : "Login to Register"}
          </Button>
        ) : (
          "N/A"
        )}
      </div>
    </div>
  );
};

export { ClassDescription };
