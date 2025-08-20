import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

interface EventTagHeaderProps {
  onCreateEventTag: () => void;
  onSearch: (value: string | undefined) => void;
}

const EventTagHeader = ({
  onCreateEventTag,
  onSearch,
}: EventTagHeaderProps) => {
  return (
    <div className="flex items-end gap-6  mt-2">
      <div className="grow">
        <TextSearch onSearch={onSearch} />
      </div>

      <Button
        onClick={onCreateEventTag}
        startIcon={<AddIcon />}
        variant="text"
        className="ml-auto "
      >
        Event Tag
      </Button>
    </div>
  );
};
export { EventTagHeader };
