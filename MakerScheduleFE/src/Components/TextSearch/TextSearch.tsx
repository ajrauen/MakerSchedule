import { TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import { useDebounce } from "@ms/hooks/useDebounce";
import { useState } from "react";

interface TextSearchProps {
  onSearch: (value: string | undefined) => void;
  debounceEnabled?: boolean;
  className?: string;
}

const TextSearch = ({
  onSearch,
  debounceEnabled = false,
  className,
}: TextSearchProps) => {
  const [searchValue, setSearchValue] = useState<string | undefined>();

  const handleSearch = (evt: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(evt.target.value);

    if (debounceEnabled) return;

    onSearch(evt.target.value);
  };

  const searchCallback = (searchString: string | undefined) => {
    onSearch(searchString);
  };

  useDebounce<string | undefined>({
    callback: searchCallback,
    value: searchValue,
    enabled: debounceEnabled,
  });

  return (
    <div className={`${className} flex items-center`}>
      <TextField
        variant="standard"
        placeholder="Search..."
        value={searchValue}
        onChange={handleSearch}
        slotProps={{
          input: {
            endAdornment: <SearchIcon />,
          },
        }}
      />
    </div>
  );
};

export { TextSearch };
