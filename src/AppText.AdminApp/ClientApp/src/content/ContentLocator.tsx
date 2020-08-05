import React from 'react';
import { Field, Formik, getIn } from 'formik';
import { Select, SelectOption } from '../common/components/form';
import { FaTimes } from 'react-icons/fa';

import { Collection } from '../collections/models';
import { useTranslation } from 'react-i18next';
import classNames from 'classnames';

interface ContentLocatorProps {
  collections: Collection[],
  collectionId?: string,
  searchTerm?: string,
  onCollectionChanged: (collectionId: string) => void,
  onSearch: (searchTerm: string) => void
}

const ContentLocator: React.FC<ContentLocatorProps> = ({ collections, collectionId, searchTerm, onCollectionChanged, onSearch }) => {
  const { t } = useTranslation('Labels');

  const collectionOptions: SelectOption[] = collections.map(c =>  ({
    value: c.id,
    label: c.name
  }));

  const initialValues = {
    collectionId: collectionId,
    searchTerm: searchTerm || ''
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
        <form className="w-75" onSubmit={handleSubmit}>
          <div className="form-row">
            <Field name="collectionId" label={t('Labels:Collection')} className="form-group col-md-4" component={Select} options={collectionOptions} onChange={onCollectionChanged} />
            <Field name="searchTerm">
              {({
                field: { name, value, ...restField }, // { name, value, onChange, onBlur }
                form: { errors, setFieldValue, submitForm }, // also values, setXXXX, handleXXXX, dirty, isValid, status, etc.
               }) => {
                const error = getIn(errors, name);

                const onClearSearchText = () => {
                  setFieldValue('searchTerm', '');
                  submitForm();
                }              
              
                 return (
                  <div className="form-group col-md-8">
                    <label>{t('Labels:KeyStartsWith')}</label>
                    <div className="input-group">
                      <input type="text" name={name} value={value||''} {...restField} className={classNames('form-control', 'w-25', { 'is-invalid': error })} />
                      <div className="input-group-append">
                        <button className="btn btn-primary" type="button" onClick={onClearSearchText}><FaTimes /></button>
                      </div>
                    </div>
                    {error && <div className="invalid-feedback">{error}</div>}
                  </div>
                 )
               }
              }
            </Field>
          </div>
        </form>
      )}
    </Formik>
  );
};
export default ContentLocator;
