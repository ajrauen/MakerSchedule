import type { EventOffering } from "@ms/types/event.types";

interface ClassDescriptionProps {
  event: EventOffering;
}

const ClassDescription = ({ event }: ClassDescriptionProps) => {
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
    </div>
  );
};

export { ClassDescription };
