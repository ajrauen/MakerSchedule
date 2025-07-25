import { sendAxiosRequest } from "@ms/api/api";
import type { AdminEventsMetaData } from "@ms/types/application-metadata.types";

const BASE_METADATA_ENDPOINT = "api/metadata";

const getApplicaitonMetadata = async () => {
  const url = `${BASE_METADATA_ENDPOINT}/events`;
  const req = {
    method: "Get",
    url: url,
  };

  return await sendAxiosRequest<AdminEventsMetaData>(req);
};

export { getApplicaitonMetadata };
