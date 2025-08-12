import { sendAxiosRequest } from "@ms/api/api";
import type { AdminEventsMetaData } from "@ms/types/application-metadata.types";
import type { UserMetaData } from "@ms/types/domain-user.types";

const BASE_METADATA_ENDPOINT = "api/metadata";

const getEventMetadata = async () => {
  const url = `${BASE_METADATA_ENDPOINT}/events`;
  const req = {
    method: "Get",
    url: url,
  };

  const metadata = await sendAxiosRequest<AdminEventsMetaData>(req);

  return metadata.data;
};

const getUserMetadata = async () => {
  const url = `${BASE_METADATA_ENDPOINT}/users`;
  const req = {
    method: "Get",
    url: url,
  };

  const metadata = await sendAxiosRequest<UserMetaData>(req);

  return metadata.data;
};

export { getEventMetadata, getUserMetadata };
