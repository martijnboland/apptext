import React from 'react';
import { Field, Formik } from 'formik';
import { TextInput, Select, SelectOption } from '../common/components/form';

import { Collection } from '../collections/models';
import { useTranslation } from 'react-i18next';

interface ContentLocatorProps {
  collections: Collection[],
  collectionId?: string,
  onCollectionChanged: (collectionId: string) => void,
  onSearch: (searchTerm: string) => void
}

const ContentLocator: React.FC<ContentLocatorProps> = ({ collections, collectionId, onCollectionChanged, onSearch }) => {
  const { t } = useTranslation('Labels');

  const collectionOptions: SelectOption[] = collections.map(c =>  ({
    value: c.id,
    label: c.name
  }));

  const initialValues = {
    collectionId: collectionId,
    searchTerm: ''
  }

  const onSubmit = (values: any) => {
    onSearch(values.searchTerm);
  };

  return (
    <Formik 
      initialValues={initialValues}
      onSubmit={onSubmit}
    >
      {({handleSubmit}) => (
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <Field name="collectionId" label={t('Labels:Collection')} className="form-group col-md-6" component={Select} options={collectionOptions} onChange={onCollectionChanged} />
            <Field name="searchTerm" label={t('Labels:KeyStartsWith')} className="form-group col-md-6" component={TextInput} />
          </div>
        </form>
      )}
    </Formik>
  );
};
export default ContentLocator;
