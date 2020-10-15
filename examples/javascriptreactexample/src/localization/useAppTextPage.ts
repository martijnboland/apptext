import { useQuery } from 'urql';

const AppTextPageQuery = `
  query ($language: String!, $contentKey: String) {
    pages {
      items(contentKeyStartsWith:$contentKey, first:1) {
        contentKey
        title(language:$language)
        content(language:$language)
      }
    }
  }
`;

export function useAppTextPage(contentKey: string, language: string) {
  const [{ data, fetching,  }] = useQuery({
    query: AppTextPageQuery,
    variables: { language: language, contentKey: contentKey }
  });

  const page = data && data.pages && data.pages.items.length > 0
    ? data.pages.items[0]
    : null;

  return { page, fetching };
}

