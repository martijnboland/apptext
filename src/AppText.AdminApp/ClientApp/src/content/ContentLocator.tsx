import React from 'react';
import { Field, Formik } from 'formik';
import { TextInput, Select, SelectOption } from '../common/components/form';

import { Collection } from '../collections/models';

interface ContentLocatorProps {
  collections: Collection[],
  collectionId?: string,
  onCollectionChanged: (collectionId: string) => void,
  onSearch: (searchTerm: string) => void
}

const ContentLocator: React.FC<ContentLocatorProps> = ({ collections, collectionId, onCollectionChanged, onSearch }) => {

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
      render={({handleSubmit}) => (
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <Field name="collectionId" label="Collection" className="form-group col-md-6" component={Select} options={collectionOptions} onChange={onCollectionChanged} />
            <Field name="searchTerm" label="Key starts with" className="form-group col-md-6" component={TextInput} />
          </div>
        </form>
      )}>
    </Formik>
  );
};
export default ContentLocator;
