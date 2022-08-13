function [P,n]=makepolymat(X,deg,typ)
% computes polynomial regressor matrix for data held in X of degree deg and
% type, typ. typ is 's' for static mapping, 'd' for dynamic (delayed and
% lagged)
% 
% if X double matrix of data, then P contains n columns of the unique
% regressors up to degree deg
% NARMAX usage: compute Y, U, E matrices of lagged data then X=[Y U E]
%
%          otherwise
%  use to see regressor terms
%                 for typ ='s' - static mapping
%             X - dimension of input space 
%                 for typ ='d' - dynamic
%             X - matrix of delays/lags for y u and e [ydel udel edel;ylag ulag elag] NB SISO only
%   e.g. P=makepolymat([0 1 1;1 2 2],1,'d');
%   e.g. P=makepolymat([0 0 1;1 0 2],1,'d'); omits u terms
%           deg - polynomial degree
%           typ - 's' for static mapping, 'd' for dynamic (e.g. NARMAX)
% Use             pretty(P) to view in Matlab
%                 latex(P) to generate Latex code (not very pretty but functional)


[N,dim]=size(X);
if N<=2;
    if nargin<3;typ='static';end
    tmp=X;
    if lower(typ(1))=='s';
        if ~isequal(size(tmp),[1 1]) | tmp<=0 ;error('Input dimension must be positive scalar');end
        symbols='x';
        X=sym(zeros(1,tmp));
        for ii=1:tmp
            X(ii)=sym(['x' '(' int2str(ii) ')']);
        end
    else
        if ~isequal(size(tmp),[2 3]) | any(find(diff(tmp)<0)); error('Lags matrix must be 2x3 and lag number must equal orexceed delay');end
        symbols=['y','u','e'];
        X=sym(zeros(1,sum(tmp(2,:)-tmp(1,:))));
        cnt=0;
        for jj=1:3
            if (tmp(2,jj)-tmp(1,jj))>=0 & tmp(2,jj)+tmp(1,jj)>0;
                for ii=tmp(1,jj):tmp(2,jj)
                    cnt=cnt+1;
                    X(cnt)=sym([symbols(jj) '(k-' int2str(ii) ')']);
                end
            end
        end
    end
    P=[sym(1) X];
    dim=size(X,2);
else
    P=[ones(N,1) X];
end
D=dim;
n=npolterm(deg,dim);
T=pascal(max(dim,deg));
tmp=X;
for jj=2:deg
    TMP=[];
    ind=flipud(T(1:dim,jj));
    for ii=1:dim
        TMP=[TMP tmp(:,D-ind(ii)+1:D).*repmat(X(:,ii),1,size(tmp(:,D-ind(ii)+1:D),2))];
    end
    tmp=TMP;D=size(tmp,2);
    P=[P TMP];
end
%[size(P,2) nterms]

