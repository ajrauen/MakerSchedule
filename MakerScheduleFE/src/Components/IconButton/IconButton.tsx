import {
  Tooltip,
  IconButton as MuiIconButton,
  type IconButtonProps,
} from "@mui/material";
import type { TooltipProps } from "@mui/material/Tooltip";

interface IconButton extends IconButtonProps {
  tooltipProps?: Omit<TooltipProps, "children">;
  children: React.ReactNode;
}

const IconButton = ({
  tooltipProps,
  children,
  ...iconButtonProps
}: IconButton) => {
  return (
    <Tooltip title={tooltipProps?.title ?? null} {...tooltipProps}>
      <MuiIconButton {...iconButtonProps}>{children}</MuiIconButton>
    </Tooltip>
  );
};

export { IconButton };
